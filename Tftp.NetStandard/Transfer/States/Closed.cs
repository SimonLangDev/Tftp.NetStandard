namespace Tftp.NetStandard.Transfer.States
{
    class Closed : BaseState
    {
        public override void OnStateEnter()
        {
            Context.Dispose();
        }
    }
}
