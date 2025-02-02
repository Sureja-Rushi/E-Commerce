import { Api } from "@mui/icons-material";
import { api } from "../../Config/apiConfig";
import {
  CREATE_PRODUCT_FAILURE,
  CREATE_PRODUCT_REQUEST,
  CREATE_PRODUCT_SUCCESS,
  DELETE_PRODUCT_FAILURE,
  DELETE_PRODUCT_REQUEST,
  DELETE_PRODUCT_SUCCESS,
  FIND_PRODUCT_BY_ID_FAILURE,
  FIND_PRODUCT_BY_ID_REQUEST,
  FIND_PRODUCT_BY_ID_SUCCESS,
  FIND_PRODUCTS_FAILURE,
  FIND_PRODUCTS_REQUEST,
  FIND_PRODUCTS_SUCCESS,
} from "./ActionType";

export const findProducts = (reqData) => async (dispatch) => {
  dispatch({ type: FIND_PRODUCTS_REQUEST });
  const {
    color,
    size,
    minPrice,
    maxPrice,
    minDiscount,
    category,
    stock,
    sort,
    pageNumber,
    pageSize,
  } = reqData;
  try {
    const response = await api.get(
      `/api/product?color=${color}&size=${size}&minPrice=${minPrice}&maxPrice=${maxPrice}&minDiscount=${minDiscount}&category=${category}&stock=${stock}&sort=${sort}&pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
    console.log("products: ", response.data);
    dispatch({ type: FIND_PRODUCTS_SUCCESS, payload: response.data });
  } catch (error) {
    dispatch({ type: FIND_PRODUCTS_FAILURE, payload: error.message.data });
  }
};

export const findProductsById = (reqData) => async (dispatch) => {
  dispatch({ type: FIND_PRODUCT_BY_ID_REQUEST });
  const { productId } = reqData;
  try {
    const response = await api.get(`/api/product/${productId}`);
    console.log("specific product: ", response.data);

    dispatch({ type: FIND_PRODUCT_BY_ID_SUCCESS, payload: response.data });
  } catch (error) {
    dispatch({ type: FIND_PRODUCT_BY_ID_FAILURE, payload: error.message.data });
  }
};

export const createProduct = (product) => async (dispatch) => {
  try {
    dispatch({type:CREATE_PRODUCT_REQUEST})
    const response = await api.post(`/api/product`, product);
    dispatch({type:CREATE_PRODUCT_SUCCESS, payload:response.data})
    console.log("Created product: ", response.data);
  } catch (error) {
    dispatch({type:CREATE_PRODUCT_FAILURE, payload:error.message.data})
  }
}

export const deleteProduct = (productId) => async (dispatch) => {
  try {
    dispatch({type:DELETE_PRODUCT_REQUEST})
    const response = await api.delete(`/api/product/${productId}`);
    console.log("delete product: ", response.data);
    dispatch({type:DELETE_PRODUCT_SUCCESS, payload:productId})
  } catch (error) {
    dispatch({type:DELETE_PRODUCT_FAILURE, payload:error.message.data})
  }
}