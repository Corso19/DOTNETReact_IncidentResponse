import axios from "axios";
import API_KEYS from "../constants/api-keys";
const base_url = `${API_KEYS.API_URL}`;

function getSensors(){
    const url = base_url + `/sensors`;
    return axios.get(url);
}

export const SensorsService = {
    getSensors
}