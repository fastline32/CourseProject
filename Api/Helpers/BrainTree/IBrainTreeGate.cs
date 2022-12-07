using Braintree;

namespace Api.Helpers.BrainTree;

public interface IBrainTreeGate
{
    IBraintreeGateway CreateGateway();
    IBraintreeGateway GetGateway();
}