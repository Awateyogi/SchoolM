/*import axios from "axios";



const api = axios.create({
  baseURL: "http://localhost:5000/api", // ⚠️ use http if https fails
});


api.interceptors.request.use(config => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

export default api;
*/
import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:5001/api", // ⚠️ HTTPS, not HTTP
});

/*api.interceptors.request.use(config => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});
*/
export default api;
