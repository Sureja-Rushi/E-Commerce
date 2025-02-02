import {
  ADD_ITEM_TO_CART_FAILURE,
  ADD_ITEM_TO_CART_REQUEST,
  ADD_ITEM_TO_CART_SUCCESS,
  GET_CART_FAILURE,
  GET_CART_REQUEST,
  GET_CART_SUCCESS,
  REMOVE_CART_ITEM_FAILURE,
  REMOVE_CART_ITEM_REQUEST,
  REMOVE_CART_ITEM_SUCCESS,
} from "./ActionType";
import { api } from "../../Config/apiConfig";

export const getCart = () => async (dispatch) => {
  dispatch({ type: GET_CART_REQUEST });

  try {
    const response = await api.get("/api/cart");
    dispatch({ type: GET_CART_SUCCESS, payload: response.data });
    console.log("cart data: ", response.data);
  } catch (error) {
    const errorMessage = error.response?.data?.message || "Failed to get cart.";

    dispatch({ type: GET_CART_FAILURE, payload: errorMessage });
  }
};

export const addItemToCart = (reqData) => async (dispatch) => {
  dispatch({ type: ADD_ITEM_TO_CART_REQUEST });

  try {
    const response = await api.post("/api/cart/add-item", reqData);
    console.log("cart: ", response.data);
    dispatch({ type: ADD_ITEM_TO_CART_SUCCESS, payload: response.data });
  } catch (error) {
    const errorMessage =
      error.response?.data?.message || "Failed to add item to cart.";
    dispatch({ type: ADD_ITEM_TO_CART_FAILURE, payload: errorMessage });
  }
};

export const removeCartItem =
  (reqData, removeAll = false) =>
  async (dispatch) => {
    dispatch({ type: REMOVE_CART_ITEM_REQUEST });

    try {
      const response = await api.delete("/api/cart/removeItem", {
        data: reqData,
        params: { removeAll },
      });
      dispatch({
        type: REMOVE_CART_ITEM_SUCCESS,
        payload: response.data.message,
      });
      console.log("remove: ", response.data);
    } catch (error) {
      dispatch({
        type: REMOVE_CART_ITEM_FAILURE,
        payload: error.response?.data?.message || error.message,
      });
    }
  };
