namespace QMSWebApplication.ViewModels.System.MeasData
{
    public class MeasDataValue
    {
        public required int CharacteristicId { get; set; }

        public required List<float> CharacteristicValue { get; set; }
    }
}
