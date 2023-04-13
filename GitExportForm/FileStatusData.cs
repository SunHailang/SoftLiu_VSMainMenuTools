namespace GitExportForm
{
    public enum FileRecordType
    {
        None,
        NewType,
        DeleteType,
        ModifyType,
    }
    public class FileStatusData
    {
        public string FilePath = "";
        public bool IsSelect = false;
        public FileRecordType RecordType = FileRecordType.NewType;
    }
}
