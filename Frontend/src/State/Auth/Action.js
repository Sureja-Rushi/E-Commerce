import axios from "axios"
import { API_BASE_URL } from "../../Config/apiConfig";
import { GET_USER_FAILURE, GET_USER_REQUEST, GET_USER_SUCCESS, LOGIN_FAILURE, LOGIN_REQUEST, LOGIN_SUCCESS, LOGOUT, REGISTER_FAILURE, REGISTER_REQUEST, REGISTER_SUCCESS } from "./ActionType";
// import Cookies from "js-cookie"

// const token = Cookies.get("AuthToken");

const saveToken = (token) => {
    if (token) {
      localStorage.setItem("AuthToken", token);
    }
  };
  
  // Helper to remove token
  const removeToken = () => {
    localStorage.removeItem("AuthToken");
  };

const registerRequest = () => ({type: REGISTER_REQUEST});
const registerSuccess = (user) => ({type: REGISTER_SUCCESS, payload:user});
const registerFailure = (error) => ({type: REGISTER_FAILURE, payload:error});

export const register = (userData) => async (dispatch) =>{

    dispatch(registerRequest())

    try{
        const response = await axios.post(`${API_BASE_URL}/api/auth/register`, userData);
        const user = response.data;
        const { token } = response.data;
        console.log("Register response: ",user);
        saveToken(token);
        dispatch(registerSuccess(user.token))
    }
    catch(error){
        dispatch(registerFailure(error.message))
    }
}

const loginRequest = () => ({type: LOGIN_REQUEST});
const loginSuccess = (user) => ({type: LOGIN_SUCCESS, payload:user});
const loginFailure = (error) => ({type: LOGIN_FAILURE, payload:error});

export const login = (userData) => async (dispatch) =>{

    dispatch(loginRequest())

    try{
        const response = await axios.post(`${API_BASE_URL}/api/auth/login`, userData);
        const user = response.data;
        const { token } = response.data; // Assume token is returned in response
    console.log("Login response: ", response.data);

    // Save token to localStorage
    saveToken(token);
        dispatch(loginSuccess(user.token))
    }
    catch(error){
        dispatch(loginFailure(error.message))
    }
}

const GetUserRequest = () => ({type: GET_USER_REQUEST});
const GetUserSuccess = (user) => ({type: GET_USER_SUCCESS, payload:user});
const GetUserFailure = (error) => ({type: GET_USER_FAILURE, payload:error});

export const getUser = () => async (dispatch) =>{

    dispatch(GetUserRequest())

    try{
        const token = localStorage.getItem("AuthToken");
        const response = await axios.get(`${API_BASE_URL}/api/auth`,{
            headers: {
              Authorization: `Bearer ${token}`, // Attach token to Authorization header
            },
        });
        const user = response.data;
        console.log("Get user response: ",user);
        dispatch(GetUserSuccess(user))
    }
    catch(error){
        dispatch(GetUserFailure(error.message))
    }
}

export const logout = () => (dispatch) => {
    removeToken();
    dispatch({type:LOGOUT, payload:null})
}