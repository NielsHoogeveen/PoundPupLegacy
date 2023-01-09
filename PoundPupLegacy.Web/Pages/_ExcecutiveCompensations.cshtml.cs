using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace PoundPupLegacy.Web.Pages
{
    public class _ExcecutiveCompensationsModel : PageModel
    {
        public void OnGet(NpgsqlConnection connection)
        {
        }
    }
}
