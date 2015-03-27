namespace Ember.Database.Common.Interfaces
{
    public interface IStoredProcedureInvoker
    {
        void Invoke(string execStatement, params object[] parameters);
    }
}
