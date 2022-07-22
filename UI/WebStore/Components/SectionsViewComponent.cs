using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public SectionsViewComponent(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        public IViewComponentResult Invoke(string SectionId)
        {
            //SectionId = HttpContext.Request.Query["SectionId"]; Деменастрация как получить что-либо из контекста

            var section_id = int.TryParse(SectionId, out var id) ? id : (int?)null;

            var sections = GetSectionViewModel(section_id, out var parent_section_id);

            var model = new SelectableSectionsViewModel
            {
                Sections = sections,
                SectionId = section_id,
                ParentSectionId = parent_section_id
            };

            return View(model);
        }

        private IEnumerable<SectionViewModel> GetSectionViewModel(int? section_id, out int? parent_section_id)
        {
            parent_section_id = null;
            var sections = _ProductData.GetSections();

            var parent_sections = sections.Where(s => s.ParentId is null);

            var parent_sections_view = parent_sections
                .Select(s => new SectionViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Order = s.Order,
                })
                .ToList();

            foreach (var parent_section in parent_sections_view)
            {
                var childs = sections.Where(s => s.ParentId == parent_section.Id);
                
                foreach (var child_section in childs)
                {
                    if (child_section.Id == section_id)
                        parent_section_id = parent_section.Id;
                    parent_section.ChildSection.Add(new SectionViewModel
                    {
                        Id = child_section.Id,
                        Name = child_section.Name,
                        Order = child_section.Order,
                        Parent = parent_section
                    });
                }

                parent_section.ChildSection.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            }

            parent_sections_view.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));

            return parent_sections_view;
        }
    }
}
