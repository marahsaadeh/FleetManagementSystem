using System.Data;

namespace Fleet_Management
{
    public class DataTableDto
    {
        public List<string> Columns { get; set; } = new List<string>();
        public List<Dictionary<string, object?>> Rows { get; set; } = new List<Dictionary<string, object?>>();

        public DataTableDto() { }

        public DataTableDto(DataTable dataTable)
        {
           
            Columns = dataTable.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToList();

           
            Rows = dataTable.Rows.Cast<DataRow>()
                .Select(row => dataTable.Columns.Cast<DataColumn>()
                .ToDictionary(col => col.ColumnName, col => row[col] ?? DBNull.Value))
                .ToList();
        }

      
        public DataTable ToDataTable()
        {
            var dataTable = new DataTable();

           
            foreach (var column in Columns)
            {
                dataTable.Columns.Add(column);
            }

         
            foreach (var row in Rows)
            {
                var dataRow = dataTable.NewRow();
                foreach (var column in Columns)
                {
                    dataRow[column] = row[column] ?? DBNull.Value;
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }

}


