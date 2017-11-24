using Tftp.NetStandard.Commands;

namespace Tftp.NetStandard.Transfer.States
{
    class SendOptionAcknowledgementBase : StateThatExpectsMessagesFromDefaultEndPoint
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            SendAndRepeat(new OptionAcknowledgement(Context.NegotiatedOptions.ToOptionList()));
        }

        public override void OnError(Error command)
        {
            Context.SetState(new ReceivedError(command));
        }

        public override void OnCancel(TftpErrorPacket reason)
        {
            Context.SetState(new CancelledByUser(reason));
        }
    }
}
