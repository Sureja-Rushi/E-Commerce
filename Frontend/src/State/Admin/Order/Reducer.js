import {
  DELETE_ORDER_FAILURE,
  DELETE_ORDER_REQUEST,
  DELETE_ORDER_SUCCESS,
  GET_ORDERS_FAILURE,
  GET_ORDERS_REQUEST,
  GET_ORDERS_SUCCESS,
  UPDATE_ORDER_STATUS_REQUEST,
  UPDATE_ORDER_STATUS_SUCCESS,
} from "./ActionType";

const initialState = {
  loading: false,
  orders: [],
  error: "",
};

export const adminOrderReducer = (state = initialState, action) => {
  switch (action.type) {
    case GET_ORDERS_REQUEST:
      return {
        ...state,
        loading: true,
      };

    case GET_ORDERS_SUCCESS:
      return {
        error: "",
        loading: false,
        orders: action.payload,
      };

    case GET_ORDERS_FAILURE:
      return {
        loading: false,
        orders: [],
        error: action.payload,
      };

    case UPDATE_ORDER_STATUS_REQUEST:
      return {
        ...state,
        loading: true,
      };

    case UPDATE_ORDER_STATUS_SUCCESS:
      return {
        ...state,
        message: action.payload,
        loading: false,
      };

    case DELETE_ORDER_REQUEST:
      return {
        ...state,
        loading: true,
      };
    case DELETE_ORDER_SUCCESS:
      return {
        ...state,
        loading: false,
        orders: state.orders.filter((order) => order.id !== action.payload),
      };

    case DELETE_ORDER_FAILURE:
      return {
        ...state,
        loading: false,
        error: action.payload,
      };
    default:
        return state;
  }
};
