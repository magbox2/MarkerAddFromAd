using System;
using System.Data;
using System.Data.Common;


namespace MarkerAddFromAd
{
    public static class FastCreator
    {
        static string DbName { get; set; }
        static string ConStr { get; set; }
        static string DbFactoryStr { get; set; }
        static string DbProviderStr { get; set; }
        static DbProviderFactory _factory;

        static FastCreator()
        {
            DbName = "beehard5";
            DbFactoryStr = "System.Data.SqlClient";
            _factory = DbProviderFactories.GetFactory(DbFactoryStr);
            ConStr = "server=AL-APP003;database=beehard5;Integrated Security=SSPI;";
        }

        public static string CreateConStr(string provider, string dataSource)
        {
            DbConnectionStringBuilder conStr = _factory.CreateConnectionStringBuilder();
            conStr["Provider"] = provider;
            conStr["Data Source"] = dataSource;
            return conStr.ConnectionString;
        }
        //***************************************************************************
        // Соединения
        //***************************************************************************

        // По умолчанию берет из конфига
        public static DbConnection CreateConnection()
        {
            return CreateConnection(ConStr);
        }
        /// <summary>
        ///  Создаем соединение
        /// </summary>
        /// <param name="conStr">Строка соединения</param>
        /// <returns>DbConnection</returns>
        public static DbConnection CreateConnection(string conStr)
        {
            DbConnection connection = _factory.CreateConnection();
            connection.ConnectionString = conStr;
            return connection;
        }
        //***************************************************************************
        // Комманды
        //***************************************************************************

        /// <summary>
        /// Создаем комманду с соединением по умолчанию
        /// </summary>
        /// <param name="comStr">Строка комманды</param>
        /// <returns>DbCommand</returns>
        public static DbCommand CreateConCommand(string comStr)
        {
            DbCommand cmd = CreateConnection().CreateCommand();
            cmd.CommandText = comStr;
            return cmd;
        }
        /// <summary>
        /// Создать комманду которая ничего не знает о соединении
        /// </summary>
        /// <param name="comStr">Строка комманды</param>
        /// <returns>DbCommand</returns>
        public static DbCommand CreateCommand(string comStr)
        {
            DbCommand cmd = _factory.CreateCommand();
            cmd.CommandText = comStr;
            return cmd;
        }
        /// <summary>
        /// Создать комманду с соединением
        /// </summary>
        /// <param name="comStr">Строка комманды</param>
        /// <param name="conStr">Строка соединения</param>
        /// <returns></returns>
        public static DbCommand CreateCommand(string comStr, string conStr)
        {
            DbCommand cmd = CreateConnection(conStr).CreateCommand();
            cmd.CommandText = comStr;
            return cmd;
        }
        /// <summary>
        /// Создать комманду и передать соединение
        /// </summary>
        /// <param name="comStr">Строка комманды</param>
        /// <param name="dbCon">Соединение</param>
        /// <returns>DbCommand</returns>
        public static DbCommand CreateCommand(string comStr, DbConnection dbCon)
        {
            DbCommand cmd = dbCon.CreateCommand();
            cmd.CommandText = comStr;
            return cmd;
        }
        //***************************************************************************
        // Адаптеры
        //***************************************************************************
        /// <summary>
        /// Адаптер с соединением по умолчанию, получает только selectCommand
        /// </summary>
        /// <param name="selectCom">Строка выбора</param>
        /// <returns>DbDataAdapter</returns>
        public static DbDataAdapter CreateConAdapter(string selectCom)
        {
            DbDataAdapter adapter = _factory.CreateDataAdapter();
            adapter.SelectCommand = CreateConCommand(selectCom);
            return adapter;
        }

        // Создаем адаптер
        public static DbDataAdapter CreateAdapter()
        {
            DbDataAdapter adapter = _factory.CreateDataAdapter();
            return adapter;
        }
        // Адаптер не знающий о соединении
        public static DbDataAdapter CreateAdapter(string selectCom)
        {
            DbDataAdapter adapter = _factory.CreateDataAdapter();
            adapter.SelectCommand = CreateCommand(selectCom);
            return adapter;
        }
        /// <summary>
        /// Адаптер создающий SelectCommand и соединение
        /// </summary>
        /// <param name="selectCom">Строка выбора</param>
        /// <param name="conStr">Строка соединения</param>
        /// <returns>DbDataAdapter</returns>
        public static DbDataAdapter CreateAdapter(string selectCom, string conStr)
        {

            DbDataAdapter adapter = _factory.CreateDataAdapter();
            adapter.SelectCommand = CreateCommand(selectCom, conStr);
            return adapter;
        }
        public static DbDataAdapter CreateAdapter(string selectCom, DbConnection dbCon)
        {
            DbDataAdapter adapter = _factory.CreateDataAdapter();
            adapter.SelectCommand = CreateCommand(selectCom, dbCon);
            return adapter;
        }
        public static DbDataAdapter CreateAdapter(DbCommand dbSelectCom)
        {
            DbDataAdapter adapter = _factory.CreateDataAdapter();
            adapter.SelectCommand = dbSelectCom;
            return adapter;
        }
        //***************************************************************************
        // Создать столбецы
        //***************************************************************************
        public static DataColumn CreateColumn(string name, Type type)
        {
            DataColumn col = new DataColumn();
            col.DataType = type;
            col.ColumnName = name;
            return col;
        }
        public static DataColumn CreateColumn(string name, Type type, bool unique)
        {
            DataColumn col = CreateColumn(name, type);
            col.Unique = unique;
            return col;
        }
        public static DataColumn CreateColumn(string name, Type type, bool unique, bool autoIncrement)
        {
            DataColumn col = CreateColumn(name, type, unique);
            col.AutoIncrement = autoIncrement;
            if (autoIncrement == true)
            {
                col.AutoIncrementStep = -1;
                col.AutoIncrementSeed = -1;
            }
            return col;
        }
        //***************************************************************************
        // Создать параметры
        //***************************************************************************
        public static DbParameter CreateParam()
        {
            DbParameter par = _factory.CreateParameter();
            return par;
        }

        public static DbParameter CreateParam(string paramName, string mappedCol, Type type)
        {
            DbParameter par = _factory.CreateParameter();
            par.DbType = TypeToDbType(type);
            par.ParameterName = paramName;
            par.SourceColumn = mappedCol;
            return par;
        }
        public static DbParameter CreateParam(string paramName, string mappedCol, DbType type)
        {
            DbParameter par = _factory.CreateParameter();
            par.DbType = type;
            par.ParameterName = paramName;
            par.SourceColumn = mappedCol;
            return par;
        }

        public static DbParameter CreateParam(string paramName, string mappedCol, DbType type, int size)
        {
            DbParameter par = CreateParam(paramName, mappedCol, type);
            par.Size = size;
            return par;
        }

        public static DbParameter CreateParam(string paramName, string mappedCol, Type type, int size)
        {
            DbParameter par = CreateParam(paramName, mappedCol, type);
            par.Size = size;
            return par;
        }

        public static DbParameter CreateParam(DataColumn col)
        {
            DbParameter par = CreateParam("@" + col.ColumnName, col.ColumnName, col.DataType);
            return par;
        }
        //***************************************************************************
        // Комманд билдер
        //***************************************************************************
        public static DbCommandBuilder CreateCommandBuilder(DbDataAdapter adapter)
        {
            DbCommandBuilder cmb = _factory.CreateCommandBuilder();
            cmb.DataAdapter = adapter;
            return cmb;
        }
        public static DbType TypeToDbType(Type t)
        {
            DbType dbt;
            try
            {
                dbt = (DbType)Enum.Parse(typeof(DbType), t.Name);
            }
            catch
            {
                dbt = DbType.Object;
            }
            return dbt;
        }
    }
}
