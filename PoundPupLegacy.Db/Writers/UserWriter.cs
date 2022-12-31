namespace PoundPupLegacy.Db.Writers;

internal class UserWriter : DatabaseWriter<User>, IDatabaseWriter<User>
{
    private const string ID = "id";
    private const string CREATED_DATE_TIME = "created_date_time";
    private const string ABOUT_ME = "about_me";
    private const string ANIMAL_WITHIN = "animal_within";
    private const string RELATION_TO_CHILD_PLACEMENT = "relation_to_child_placement";
    private const string EMAIL = "email";
    private const string PASSWORD = "password";
    private const string AVATAR = "avatar";
    public static DatabaseWriter<User> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "user",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CREATED_DATE_TIME,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
                new ColumnDefinition{
                    Name = ABOUT_ME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = ANIMAL_WITHIN,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = RELATION_TO_CHILD_PLACEMENT,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = EMAIL,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = PASSWORD,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = AVATAR,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new UserWriter(command);

    }

    internal UserWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(User user)
    {
        try
        {
            WriteValue(user.Id, ID);
            WriteValue(user.CreatedDateTime, CREATED_DATE_TIME);
            WriteValue(user.Email, EMAIL);
            WriteValue(user.Password, PASSWORD);
            WriteNullableValue(user.AboutMe, ABOUT_ME);
            WriteNullableValue(user.AnimalWithin, ANIMAL_WITHIN);
            WriteNullableValue(user.RelationToChildPlacement, RELATION_TO_CHILD_PLACEMENT);
            WriteNullableValue(user.Avatar, AVATAR);
            _command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
