import AdyenCheckout from '@adyen/adyen-web';
import '@adyen/adyen-web/dist/adyen.css';

const configuration = {
    //paymentMethodsResponse: paymentMethodsResponse, // The `/paymentMethods` response from the server.
    //clientKey: "YOUR_CLIENT_KEY", // Web Drop-in versions before 3.10.1 use originKey instead of clientKey.
    locale: "de-DE",
    environment: "test",
    analytics: {
        enabled: false // Set to false to not send analytics data to Adyen.
    },
    //paymentMethodsConfiguration: {
    //    card: { // Example optional configuration for Cards
    //        hasHolderName: true,
    //        holderNameRequired: true,
    //        enableStoreDetails: true,
    //        hideCVC: false, // Change this to true to hide the CVC field for stored cards
    //        name: 'Credit or debit card',
    //        onSubmit: () => { }, // onSubmit configuration for card payments. Overrides the global configuration.
    //    }
    //}
};


export async function startPaymentAsync(clientKey, paymentMethods, checkoutTickets) {
    configuration.clientKey = clientKey;
    configuration.paymentMethodsResponse = JSON.parse(paymentMethods);

    configuration.onSubmit = (state, dropin) => {
        // Global configuration for onSubmit
        // Your function calling your server to make the `/payments` request
        checkoutTickets.invokeMethodAsync('MakePayment', state.data)
            .then(response => {
                let responseData = JSON.parse(response);
                if (responseData.action) {
                    // Drop-in handles the action object from the /payments response
                    dropin.handleAction(responseData.action);
                } else {
                    // Your function to show the final result to the shopper
                    //dropin.setStatus(responseData.ResultCode);
                    checkoutTickets.invokeMethodAsync('ShowResult', responseData.ResultCode, responseData.RefusalReasonCode);
                }
            })
            .catch(error => {
                throw Error(error);
            });
    };

    configuration.onAdditionalDetails = (state, dropin) => {
        // Your function calling your server to make a `/payments/details` request
        checkoutTickets.invokeMethodAsync('MakeDetailsCall', state.data)
            .then(response => {
                if (response.action) {
                    // Drop-in handles the action object from the /payments response
                    dropin.handleAction(response.action);
                } else {
                    // Your function to show the final result to the shopper
                    //dropin.setStatus(responseData.ResultCode);
                    checkoutTickets.invokeMethod('ShowResult', responseData.ResultCode, responseData.RefusalReasonCode);
                }
            })
            .catch(error => {
                throw Error(error);
            });
    };

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