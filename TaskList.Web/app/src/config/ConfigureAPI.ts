import axios from 'axios';
import Routes from './ConfigureRoutes';

/**
 * Configure Axios with defaults such as baseurl and headers. Also has interceptors for failed authentication requests
 *
 */
function ConfigureAxios() {
    // axios.defaults.baseURL = window.react_localapi_location
    axios.defaults.baseURL = "https://localhost:44335";
    axios.defaults.withCredentials = true;
    // This is an example
    axios.interceptors.response.use(
        (response) => {
            return response;
        },
        (error) => {
            // if(error.response.status === 401) {
            //     console.log(error);
            //     // Unauthorized! Redirect to auth endpoint so they can log in with Azure AD.
            //     // return window.location.href = "/api/auth";
            // }

            if(error.response.status === 403){
                console.log(error);
                window.location.href = Routes.GET.UNAUTHORIZED;
            }

            return Promise.reject(error);
        }
    );
}

export default ConfigureAxios;