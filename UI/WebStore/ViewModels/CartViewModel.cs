namespace WebStore.ViewModels
{
    public class CartViewModel
    {
        /// <summary>
        /// Перечисление кортежей(ViewModel товара  его колличество)
        /// </summary>
        public IEnumerable<(ProductViewModel Product, int Quantity)> Items { get; set; }

        /// <summary>
        /// Считаем колличество товара
        /// </summary>
        public int ItemsCount => Items.Sum(p => p.Quantity);

        /// <summary>
        /// Считаем полную стоимость
        /// </summary>
        public decimal TotalSum => Items.Sum(p => p.Product.Price * p.Quantity);
    }
}
