import AdyenCheckout from '@adyen/adyen-web';
import '@adyen/adyen-web/dist/adyen.css';


//const configuration = {
//    environment: 'test', // Change to one of the environment values specified in step 4.
//    //clientKey: 'test_870be2...', // Public key used for client-side authentication: https://docs.adyen.com/development-resources/client-side-authentication
//    analytics: {
//        enabled: false // Set to false to not send analytics data to Adyen.
//    },
//    session: {
//        //id: 'CSD9CAC3...', // Unique identifier for the payment session.
//        //sessionData: 'Ab02b4c...' // The payment session data.
//    },
//    onPaymentCompleted: (result, component) => {
//        console.info(result, component);
//    },
//    onError: (error, component) => {
//        console.error(error.name, error.message, error.stack, component);
//    },
//    // Any payment method specific configuration. Find the configuration specific to each payment method:  https://docs.adyen.com/payment-methods
//    // For example, this is 3D Secure configuration for cards:
//    paymentMethodsConfiguration: {
//        card: {
//            hasHolderName: true,
//            holderNameRequired: true,
//            billingAddressRequired: true
//        }
//    }
//};

//export async function startPaymentAsync(clientKey, sessionId, sessionData) {
//    configuration.clientKey = clientKey;
//    configuration.session.id = sessionId;
//    configuration.session.sessionData = sessionData;

//    // Create an instance of AdyenCheckout using the configuration object.
//    const checkout = await AdyenCheckout(configuration);
//    // Access the available payment methods for the session.
//    console.log(checkout.paymentMethodsResponse); // => { paymentMethods: [...], storedPaymentMethods: [...] }

//    // Create an instance of the Component and mount it to the container you created.
//    const cardComponent = checkout.create('card').mount('#payment-container');

//    // TODO Handle RedirectResult
//    // https://docs.adyen.com/online-payments/web-components#handle-redirect-result
//};


const configuration = {
    //paymentMethodsResponse: paymentMethodsResponse, // The `/paymentMethods` response from the server.
    //clientKey: "YOUR_CLIENT_KEY", // Web Drop-in versions before 3.10.1 use originKey instead of clientKey.
    locale: "de-DE",
    environment: "test",
    analytics: {
        enabled: false // Set to false to not send analytics data to Adyen.
    },
    onSubmit: (state, dropin) => {
        // Global configuration for onSubmit
        // Your function calling your server to make the `/payments` request
        makePayment(state.data)
            .then(response => {
                if (response.action) {
                    // Drop-in handles the action object from the /payments response
                    dropin.handleAction(response.action);
                } else {
                    // Your function to show the final result to the shopper
                    showFinalResult(response);
                }
            })
            .catch(error => {
                throw Error(error);
            });
    },
    onAdditionalDetails: (state, dropin) => {
        // Your function calling your server to make a `/payments/details` request
        makeDetailsCall(state.data)
            .then(response => {
                if (response.action) {
                    // Drop-in handles the action object from the /payments response
                    dropin.handleAction(response.action);
                } else {
                    // Your function to show the final result to the shopper
                    showFinalResult(response);
                }
            })
            .catch(error => {
                throw Error(error);
            });
    },
    paymentMethodsConfiguration: {
        card: { // Example optional configuration for Cards
            hasHolderName: true,
            holderNameRequired: true,
            enableStoreDetails: true,
            hideCVC: false, // Change this to true to hide the CVC field for stored cards
            name: 'Credit or debit card',
            onSubmit: () => { }, // onSubmit configuration for card payments. Overrides the global configuration.
        }
    }
};


export async function startPaymentAsync(clientKey, paymentMethods) {
    configuration.clientKey = clientKey;
    configuration.paymentMethodsResponse = JSON.parse(paymentMethods);

    // Create an instance of AdyenCheckout using the configuration object.
    const checkout = await AdyenCheckout(configuration);
    // Access the available payment methods for the session.
    console.log(clientKey);
    console.log(checkout.paymentMethodsResponse);

    // Create an instance of the Component and mount it to the container you created.
    const dropin = checkout
        .create('dropin', {
            // Starting from version 4.0.0, Drop-in configuration only accepts props related to itself and cannot contain generic configuration like the onSubmit event.
            openFirstPaymentMethod: false
        })
        .mount('#dropin-container');

    // TODO Handle RedirectResult
    // https://docs.adyen.com/online-payments/web-components#handle-redirect-result
};