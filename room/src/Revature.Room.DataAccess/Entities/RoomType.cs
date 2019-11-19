namespace Revature.Room.DataAccess.Entities
{
  /// <summary>
  /// Entity for available room types: Apartment, Dorm, House, etc
  /// </summary>
  public class RoomType
  {
    public int RoomTypeId { get; set; }
    public string Type { get; set; }
  }
}
