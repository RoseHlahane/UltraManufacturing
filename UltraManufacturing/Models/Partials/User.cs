using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UltraManufacturing.Models.Entities
{
    [ModelMetadataType(typeof(UserMetadataType))]
    public partial class User
    {
    }

    public partial class UserMetadataType
    {
        
    }

}
