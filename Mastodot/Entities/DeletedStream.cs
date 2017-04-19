using System;
namespace Mastodot.Entities
{
    public class DeletedStream: IStreamEntity
    {
        public int StatusId { get; set; }
    }
}
