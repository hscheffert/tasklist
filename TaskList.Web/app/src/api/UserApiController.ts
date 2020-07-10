import { HttpApi, HttpApiRequestOptions, AxiosResponse } from './ApiHelper';
import UserDTO from '../models/generated/UserDTO';


class UserApiControllerInternal {
    // get: api/users
    public RouteGetAll = () => `api/users`;
    public getAll(requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<UserDTO[]>> {
        let url = this.RouteGetAll();
        return HttpApi.RestRequest<any, UserDTO[]>(null, 'get', url, requestOptions);
    }
    // get: api/users/getAllActive
    public RouteGetAllActive = () => `api/users/getAllActive`;
    public getAllActive(requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<UserDTO[]>> {
        let url = this.RouteGetAllActive();
        return HttpApi.RestRequest<any, UserDTO[]>(null, 'get', url, requestOptions);
    }
    // get: api/users/${encodeURIComponent(id)}
    public RouteGet = (id: string) => `api/users/${encodeURIComponent(id)}`;
    public get(id: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<UserDTO>> {
        let url = this.RouteGet(id);
        return HttpApi.RestRequest<any, UserDTO>(null, 'get', url, requestOptions);
    }
    // post: api/users
    public RoutePost = (dto: UserDTO) => `api/users`;
    public post(dto: UserDTO, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<string>> {
        let url = this.RoutePost(dto);
        return HttpApi.RestRequest<any, string>(dto, 'post', url, requestOptions);
    }
    // put: api/users/toggle/${encodeURIComponent(id)}
    public RouteToggle = (id: string) => `api/users/toggle/${encodeURIComponent(id)}`;
    public toggle(id: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<void>> {
        let url = this.RouteToggle(id);
        return HttpApi.RestRequest<any, void>(null, 'put', url, requestOptions);
    }
    // get: api/users/getActiveSupervisors
    public RouteGetActiveSupervisors = () => `api/users/getActiveSupervisors`;
    public getActiveSupervisors(requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<UserDTO[]>> {
        let url = this.RouteGetActiveSupervisors();
        return HttpApi.RestRequest<any, UserDTO[]>(null, 'get', url, requestOptions);
    }
}
var UserApiController = new UserApiControllerInternal();
export default UserApiController;

