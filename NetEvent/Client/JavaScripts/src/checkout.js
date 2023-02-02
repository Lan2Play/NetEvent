import AdyenCheckout from '@adyen/adyen-web';
import '@adyen/adyen-web/dist/adyen.css';


const configuration = {
    environment: 'test', // Change to one of the environment values specified in step 4.
    //clientKey: 'test_870be2...', // Public key used for client-side authentication: https://docs.adyen.com/development-resources/client-side-authentication
    analytics: {
        enabled: false // Set to false to not send analytics data to Adyen.
    },
    session: {
        //id: 'CSD9CAC3...', // Unique identifier for the payment session.
        //sessionData: 'Ab02b4c...' // The payment session data.
    },
    onPaymentCompleted: (result, component) => {
        console.info(result, component);
    },
    onError: (error, component) => {
        console.error(error.name, error.message, error.stack, component);
    },
    // Any payment method specific configuration. Find the configuration specific to each payment method:  https://docs.adyen.com/payment-methods
    // For example, this is 3D Secure configuration for cards:
    paymentMethodsConfiguration: {
        card: {
            hasHolderName: true,
            holderNameRequired: true,
            billingAddressRequired: true
        }
    }
};

const jsInterop = jsInterop || {};

jsInterop.startPayment = async function (clientKey, sessionId, sessionData) {
    configuration.clientKey = clientKey;
    configuration.session.id = sessionId;
    configuration.session.sessionData = sessionData;

    // Create an instance of AdyenCheckout using the configuration object.
    const checkout = await AdyenCheckout(configuration);
    // Access the available payment methods for the session.
    console.log(checkout.paymentMethodsResponse); // => { paymentMethods: [...], storedPaymentMethods: [...] }

    // Create an instance of the Component and mount it to the container you created.
    const cardComponent = checkout.create('card').mount('#payment-container');

    // TODO Handle RedirectResult
    // https://docs.adyen.com/online-payments/web-components#handle-redirect-result
};