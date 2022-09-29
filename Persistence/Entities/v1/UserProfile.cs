using Persistence.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace Persistence.Entities.v1
{
    /// <summary>
    /// UserProfile Entity
    /// </summary>
    public class UserProfile : BaseEntity
    {
        /// <summary>
        /// Id of UserProfile
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The profile picture url
        /// </summary>
        public string? ProfilePictureUrl { get; set; }

        /// <summary>
        /// True for enabled notification. Otherwise false.
        /// </summary>
        public bool IsEmailNotificationEnabled { get; set; }

        /// <summary>
        /// Nav prop
        /// </summary>
        public Guid UserId { get; set; }
    }
}