using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StudentMngt.Domain.Entities
{
    public abstract class DomainEntity<TKey>
    {
        [Key]
        public TKey Id { get; set; }
    }
}
