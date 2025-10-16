namespace SalesWebMVC.Models.ViewModels
{
    public class ProfilesIndexViewModel
    {
        public string Id { get; set; }      // ID do usuário, útil para ações como Editar
        public string Login { get; set; }
        public string Name { get; set; }    // Nome do usuário
        public string Email { get; set; }   // Email do usuário
        public string Role { get; set; }    // Papel (role) do usuário


    }
}
