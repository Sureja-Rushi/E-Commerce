import { api } from "../../Config/apiConfig";
import {
    GET_PAYMENT_STATUS_FAILURE,
    GET_PAYMENT_STATUS_REQUEST,
  GET_PAYMENT_STATUS_SUCCESS,
  INITIATE_PAYMENT_FAILURE,
  INITIATE_PAYMENT_REQUEST,
  INITIATE_PAYMENT_SUCCESS,
  PAYMENT_CALLBACK_FAILURE,
  PAYMENT_CALLBACK_REQUEST,
  PAYMENT_CALLBACK_SUCCESS,
} from "./ActionType";

export const initiatePayment = (orderId) => async (dispatch) => {
  dispatch({ type: INITIATE_PAYMENT_REQUEST });

  try {
    const response = await api.post(`/api/payment/${orderId}/initiate`);
    console.log("payment: ", response.data);
    localStorage.setItem("paymentUrl", response.data.url);
    dispatch({
        type: INITIATE_PAYMENT_SUCCESS,
        payload: response.data.url,
      });
    if(response.data.url){
        window.location.href = response.data.url;
    }
  } catch (error) {
    dispatch({
      type: INITIATE_PAYMENT_FAILURE,
      payload: error.response?.data?.error || "Failed to initiate payment.",
    });
  }
};

export const handlePaymentCallback = (sessionId) => async (dispatch) => {
    dispatch({ type: PAYMENT_CALLBACK_REQUEST });
  
    try {
      const response = await api.post(`/api/payment/callback`, { sessionId });
      dispatch({
        type: PAYMENT_CALLBACK_SUCCESS,
        payload: response.data.message,
      });
    } catch (error) {
      dispatch({
        type: PAYMENT_CALLBACK_FAILURE,
        payload: error.response?.data?.error || "Failed to handle payment callback.",
      });
    }
  };

  export const getPaymentStatus = (paymentId) => async (dispatch) => {
    dispatch({ type: GET_PAYMENT_STATUS_REQUEST });
  
    try {
      const response = await api.get(`/api/payment/${paymentId}/status`);
      dispatch({
        type: GET_PAYMENT_STATUS_SUCCESS,
        payload: response.data.status,
      });
    } catch (error) {
      dispatch({
        type: GET_PAYMENT_STATUS_FAILURE,
        payload: error.response?.data?.error || "Failed to fetch payment status.",
      });
    }
  };