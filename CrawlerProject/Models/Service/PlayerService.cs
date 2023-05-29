using CrawlerProject.Models.DTO;
using Microsoft.Data.SqlClient;

namespace CrawlerProject.Models.Service
{
    public class PlayerService
    {
        private readonly SqlConnection _connectionString;
        public PlayerService()
        {
            _connectionString = ConnectionSettingService.connection;
        }
        public ObjectItem AddPlayer(AddPlayer model)
        {
            var newGuid = Guid.NewGuid();
            SqlConnection sqlconnection = _connectionString;
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.Connection = _connectionString;

            sqlcommand.Parameters.AddWithValue("@PlayerName",
                model.PlayerName);
            sqlcommand.Parameters.AddWithValue("@LastName",
                model.LastName);
            sqlcommand.Parameters.AddWithValue("@BirthDay",
                model.BirthDay);
            sqlcommand.Parameters.AddWithValue("@IsDelete",
                false);
            sqlcommand.Parameters.AddWithValue("@CreatedDate",
                DateTime.Now);
            sqlcommand.Parameters.AddWithValue("@TeamId",
                model.TeamId);
            sqlcommand.Parameters.AddWithValue("@Id",
                newGuid);

            sqlcommand.CommandText = "INSERT INTO Players (Id,IsDelete,CreatedDate,BirthDay,LastName,PlayerName,TeamId) VALUES (@Id,@IsDelete,@CreatedDate,@BirthDay,@LastName,@PlayerName,@TeamId)";

            sqlconnection.Open();
            sqlcommand.ExecuteNonQuery();
            sqlconnection.Close();
            return new ObjectItem { Id = newGuid };
        }

        public bool UpdatePlayer(UpdatePlayer param)
        {
            if (!string.IsNullOrEmpty(param.PlayerName) && !string.IsNullOrEmpty(param.LastName))
            {
                SqlConnection sqlconnection = _connectionString;
                SqlCommand sqlcommand = new SqlCommand();
                sqlconnection.Open();
                sqlcommand.CommandText = "update Players set PlayerName = @playerName, LastName = @lastName, UpdatedDate = @updatedDate, TeamId = @teamId, BirthDay = @birthday where Id = @Id";
                sqlcommand.Parameters.AddWithValue("@playerName", param.PlayerName);
                sqlcommand.Parameters.AddWithValue("@lastName", param.LastName);
                sqlcommand.Parameters.AddWithValue("@Id", param.PlayerId);
                sqlcommand.Parameters.AddWithValue("@updatedDate", DateTimeOffset.Now);
                sqlcommand.Parameters.AddWithValue("@birthday", param.Birthday);
                sqlcommand.Parameters.AddWithValue("@teamId", param.TeamId);

                sqlcommand.Connection = sqlconnection;
                sqlcommand.ExecuteNonQuery();
                sqlconnection.Close();
                return true;
            }
            return false;
        }
        public bool UpdateTeamToPlayer(UpdatePlayerTeams param)
        {
            if (param.TeamId != Guid.Empty)
            {
                SqlConnection sqlconnection = _connectionString;
                SqlCommand sqlcommand = new SqlCommand();
                sqlconnection.Open();
                sqlcommand.CommandText = "update Players set TeamId = @teamId where Id = @Id";
                sqlcommand.Parameters.AddWithValue("@teamId", param.TeamId);
                sqlcommand.Parameters.AddWithValue("@Id", param.PlayerId);
                sqlcommand.Connection = sqlconnection;
                sqlcommand.ExecuteNonQuery();
                sqlconnection.Close();
                return true;
            }
            return false;
        }
        public List<GetPlayer> GetListPlayer()
        {
            SqlConnection sqlconnection = _connectionString;
            SqlCommand sqlcommand = new SqlCommand();
            sqlconnection.Open();

            sqlcommand.CommandText = "select * from Players Where IsDelete = 0 ORDER BY [CreatedDate] ASC";
            sqlcommand.Connection = sqlconnection;
            SqlDataReader sqlDataReader = sqlcommand.ExecuteReader();
            List<GetPlayer> playerList = new List<GetPlayer>();

            while (sqlDataReader.Read())
            {
                GetPlayer player = new GetPlayer();
                player.Id = (Guid)sqlDataReader[0];
                player.LastName = sqlDataReader[5].ToString();
                player.Name = sqlDataReader[6].ToString();
                player.BirthDay = DateTime.Parse(sqlDataReader[4].ToString());
                string teamId = sqlDataReader[7].ToString();
                string teamName = null;
                if (!string.IsNullOrEmpty(teamId))
                {
                    teamName = TeamService.GetTeamName(new ObjectItem()
                    {
                        Id = new Guid(teamId)
                    });
                    if (!string.IsNullOrEmpty(teamName))
                    {
                        player.TeamInfo = new GetTeamName()
                        {
                            TeamName = teamName
                        };
                    }
                }
                playerList.Add(player);
            }
            sqlconnection.Close();
            return playerList;
        }
        public List<GetPlayer> GetListPlayerByTeamId(ObjectItem param)
        {
            List<GetPlayer> playerList = new List<GetPlayer>();
            if (param.Id != Guid.Empty)
            {
                SqlConnection sqlconnection = _connectionString;
                SqlCommand sqlcommand = new SqlCommand();
                sqlconnection.Open();

                sqlcommand.CommandText = "select * from Players Where IsDelete = 0 and TeamId = @Id";
                sqlcommand.Parameters.AddWithValue("@Id", param.Id);
                sqlcommand.Connection = sqlconnection;
                SqlDataReader sqlDataReader = sqlcommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    GetPlayer player = new GetPlayer();
                    player.LastName = sqlDataReader[8].ToString();
                    player.Name = sqlDataReader[9].ToString();
                    player.BirthDay = DateTime.Parse(sqlDataReader[7].ToString());
                    playerList.Add(player);
                }
            }

            return playerList;
        }

        public GetPlayer GetPlayerById(ObjectItem param)
        {
            GetPlayer player = new GetPlayer();
            if (param.Id != Guid.Empty)
            {
                SqlConnection sqlconnection = _connectionString;
                SqlCommand sqlcommand = new SqlCommand();
                sqlconnection.Open();

                sqlcommand.CommandText = "SELECT Id,PlayerName, LastName,BirthDay, TeamId  FROM Players Where IsDelete = 0 and Id = @Id";
                sqlcommand.Parameters.AddWithValue("@Id", param.Id);
                sqlcommand.Connection = sqlconnection;
                SqlDataReader sqlDataReader = sqlcommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    player.Id = (Guid)sqlDataReader[0];
                    player.Name = sqlDataReader[1].ToString();
                    player.LastName = sqlDataReader[2].ToString();
                    player.BirthDay = DateTime.Parse(sqlDataReader[3].ToString());

                    string teamId = sqlDataReader[4].ToString();
                    string teamName = null;
                    if (!string.IsNullOrEmpty(teamId))
                    {
                        teamName = TeamService.GetTeamName(new ObjectItem()
                        {
                            Id = new Guid(teamId)
                        });
                        if (!string.IsNullOrEmpty(teamName))
                        {
                            player.TeamInfo = new GetTeamName()
                            {
                                TeamName = teamName,
                                TeamId = Guid.Parse(teamId)
                            };
                        }
                    }
                }
                sqlconnection.Close();
            }

            return player;
        }

        public bool Delete(ObjectItem param)
        {
            SqlConnection sqlconnection = _connectionString;
            SqlCommand sqlcommand = new SqlCommand();
            sqlconnection.Open();
            sqlcommand.CommandText = "update Players set IsDelete = 1 where Id = @Id";
            sqlcommand.Parameters.AddWithValue("@Id", param.Id);
            sqlcommand.Connection = sqlconnection;
            sqlcommand.ExecuteNonQuery();
            sqlconnection.Close();
            return true;
        }
    }
}
