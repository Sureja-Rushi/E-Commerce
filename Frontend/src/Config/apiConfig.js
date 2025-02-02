import axios from "axios";

export const API_BASE_URL = "https://localhost:7040"

const jwt=localStorage.getItem("AuthToken");

export const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        "Authorization": `Bearer ${jwt}`,
        "Content-Type": "application/json",
    }
})