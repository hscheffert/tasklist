import { HttpApi, HttpApiRequestOptions, AxiosResponse } from './ApiHelper';
import UserDTO from '../models/generated/UserDTO';


class AuthControllerInternal {
    // get: api/auth/login
    public RouteLogin = () => `api/auth/login`;
    public login(requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<void>> {
        let url = this.RouteLogin();
        return HttpApi.RestRequest<any, void>(null, 'get', url, requestOptions);
    }
    // get: api/auth/me
    public RouteGetMe = () => `api/auth/me`;
    public getMe(requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<UserDTO>> {
        let url = this.RouteGetMe();
        return HttpApi.RestRequest<any, UserDTO>(null, 'get', url, requestOptions);
    }
}
var AuthController = new AuthControllerInternal();
export default AuthController;

