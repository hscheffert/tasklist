import { HttpApi, HttpApiRequestOptions, AxiosResponse } from './ApiHelper';
import StaffDTO from '../models/generated/StaffDTO';


class StaffApiControllerInternal {
    // get: api/staff
    public RouteGetAll = () => `api/staff`;
    public getAll(requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<StaffDTO[]>> {
        let url = this.RouteGetAll();
        return HttpApi.RestRequest<any, StaffDTO[]>(null, 'get', url, requestOptions);
    }
    // get: api/staff/${encodeURIComponent(id)}
    public RouteGet = (id: string) => `api/staff/${encodeURIComponent(id)}`;
    public get(id: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<StaffDTO>> {
        let url = this.RouteGet(id);
        return HttpApi.RestRequest<any, StaffDTO>(null, 'get', url, requestOptions);
    }
    // post: api/staff
    public RoutePost = (dto: StaffDTO) => `api/staff`;
    public post(dto: StaffDTO, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<string>> {
        let url = this.RoutePost(dto);
        return HttpApi.RestRequest<any, string>(dto, 'post', url, requestOptions);
    }
    // put: api/staff/toggle/${encodeURIComponent(id)}
    public RouteToggle = (id: string) => `api/staff/toggle/${encodeURIComponent(id)}`;
    public toggle(id: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<void>> {
        let url = this.RouteToggle(id);
        return HttpApi.RestRequest<any, void>(null, 'put', url, requestOptions);
    }
}
var StaffApiController = new StaffApiControllerInternal();
export default StaffApiController;

