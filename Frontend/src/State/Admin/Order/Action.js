import { api } from "../../../Config/apiConfig";
import {
  DELETE_ORDER_FAILURE,
  DELETE_ORDER_REQUEST,
  DELETE_ORDER_SUCCESS,
  GET_ORDERS_FAILURE,
  GET_ORDERS_REQUEST,
  GET_ORDERS_SUCCESS,
  UPDATE_ORDER_STATUS_FAILURE,
  UPDATE_ORDER_STATUS_REQUEST,
  UPDATE_ORDER_STATUS_SUCCESS,
} from "./ActionType";

export const getOrders = () => async (dispatch) => {
  dispatch({ type: GET_ORDERS_REQUEST });
  try {
    const response = await api.get(`/api/order`);
    console.log("get all orders: ", response.data);
    dispatch({ type: GET_ORDERS_SUCCESS, payload: response.data });
  } catch (error) {
    dispatch({ type: GET_ORDERS_FAILURE, payload: error.message.data });
  }
};

export const updateOrderStatus = (orderId, status) => async (dispatch) => {
  dispatch({ type: UPDATE_ORDER_STATUS_REQUEST });

  try {
    const response = api.put(`/api/order/${orderId}/status?status=${status}`);
    console.log("update order status: ", response.data);
    dispatch({
      type: UPDATE_ORDER_STATUS_SUCCESS,
      payload: (await response).data.message,
    });
  } catch (error) {
    dispatch({
      type: UPDATE_ORDER_STATUS_FAILURE,
      payload:
        error.response?.data?.message ||
        "An error occurred while updating status.",
    });
  }
};

export const deleteOrder = (orderId) => async (dispatch) => {
  dispatch({ type: DELETE_ORDER_REQUEST });
  try {
    const response = await api.delete(`api/order/${orderId}`);
    console.log("delete order: ", response.data);
    dispatch({ type: DELETE_ORDER_SUCCESS, payload: orderId });
  } catch (error) {
    dispatch({
      type: DELETE_ORDER_FAILURE,
      payload:
        error.response?.data?.message ||
        "An error occurred while deleting the order.",
    });
  }
};
