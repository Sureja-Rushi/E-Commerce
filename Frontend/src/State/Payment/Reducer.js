import { INITIATE_PAYMENT_REQUEST, INITIATE_PAYMENT_SUCCESS } from "./ActionType";

const initialState = {
  url: "",
};

export const paymentReducer = (state = initialState, action) => {
  switch (action.type) {
    case INITIATE_PAYMENT_REQUEST:
      return {
        ...state,
      };

    case INITIATE_PAYMENT_SUCCESS:
        // localStorage.setItem("paymentUrl", action.payload);
      return {
        ...state,
        url: action.payload,
      };
    
    default:
      return state;
  }
};
