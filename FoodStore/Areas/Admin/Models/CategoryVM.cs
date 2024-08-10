
using System.ComponentModel.DataAnnotations;

namespace FoodStore.Areas.Admin.Models
{
	public class CategoryVM
	{
		public string? Id { get; set; }
		[Required]
		public string? Name { get; set; }
	}
}
