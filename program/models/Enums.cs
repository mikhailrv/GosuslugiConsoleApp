namespace GosuslugiWinForms.Models
{
public enum Role
{
    CITIZEN,
    ADMIN,
    CIVIL_SERVANT
}
    public enum ApplicationStatus
    {
        PENDING,
        COMPLETED,
        REJECTED,
        CANCELLED
    }

    public enum Operator
    {
        EQUAL,
        NOT_EQUALS,
        GREATER_THAN,
        LESS_THAN,
        BETWEEN
    }

    public enum ValueType
    {
        STRING,
        INTEGER,
        DATE,
        BOOLEAN
    }
}