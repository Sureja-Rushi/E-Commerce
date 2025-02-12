import { api } from "../../Config/apiConfig";
import {
  CREATE_ORDER_FAILURE,
  CREATE_ORDER_REQUEST,
  CREATE_ORDER_SUCCESS,
  GET_ORDER_BY_ID_FAILURE,
  GET_ORDER_BY_ID_REQUEST,
  GET_ORDER_BY_ID_SUCCESS,
  GET_ORDER_HISTORY_FAILURE,
  GET_ORDER_HISTORY_REQUEST,
  GET_ORDER_HISTORY_SUCCESS,
} from "./ActionType";

export const createOrder = (reqData) => async (dispatch) => {
  dispatch({ type: CREATE_ORDER_REQUEST });

  try {
    const response = await api.post("/api/order/create-order", reqData.OrderData);

    // if(response.status == 200) {
    //   throw new Error("Failed to create order.");
    // }

    if(response.data.order.id){
      reqData.navigate({ search: `step=3&order_id=${response.data.order.id}` });
    }

    console.log("order: ", response.data);

    dispatch({ type: CREATE_ORDER_SUCCESS, payload: response.data });
  } catch (error) {
    const errorMessage =
      error.response?.data?.message || "Failed to create order.";

    dispatch({
      type: CREATE_ORDER_FAILURE,
      payload: errorMessage,
    });
  }
};

export const getOrderById = (orderId) => async (dispatch) => {
    dispatch({ type: GET_ORDER_BY_ID_REQUEST });

    try{
        const response = await api.get(`/api/order/${orderId}`);
        console.log("Order Data: ", response.data);
        dispatch({ type: GET_ORDER_BY_ID_SUCCESS, payload: response.data });
    }catch(error){
        const errorMessage =
            error.response?.data?.message || "Failed to fetch order details.";

        // Dispatch failure action with the error message
        dispatch({
            type: GET_ORDER_BY_ID_FAILURE,
            payload: errorMessage,
        });
    }
}

export const orderHistory = () => async (dispatch) => {
  dispatch({ type: GET_ORDER_HISTORY_REQUEST });

  try{
    const response = await api.get("/api/order/history");
    console.log("Order History: ", response.data);
    dispatch({ type: GET_ORDER_HISTORY_SUCCESS, payload: response.data });
  }catch(error){
    const errorMessage =
            error.response?.data?.message || "Failed to fetch order details.";

        // Dispatch failure action with the error message
        dispatch({
            type: GET_ORDER_HISTORY_FAILURE,
            payload: errorMessage,
        });
  }
} 