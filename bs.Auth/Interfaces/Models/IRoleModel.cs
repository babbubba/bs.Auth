namespace bs.Auth.Interfaces.Models
{
    public interface IRoleModel<T>
    {
        public T Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
