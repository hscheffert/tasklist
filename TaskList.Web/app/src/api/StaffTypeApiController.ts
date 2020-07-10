import { HttpApi, HttpApiRequestOptions, AxiosResponse } from './ApiHelper';
import StaffTypeDTO from '../models/generated/StaffTypeDTO';


class StaffTypeApiControllerInternal {
    // get: api/staffTypes
    public RouteGetAll = () => `api/staffTypes`;
    public getAll(requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<StaffTypeDTO[]>> {
        let url = this.RouteGetAll();
        return HttpApi.RestRequest<any, StaffTypeDTO[]>(null, 'get', url, requestOptions);
    }
    // get: api/staffTypes/getAllActive
    public RouteGetAllActive = () => `api/staffTypes/getAllActive`;
    public getAllActive(requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<StaffTypeDTO[]>> {
        let url = this.RouteGetAllActive();
        return HttpApi.RestRequest<any, StaffTypeDTO[]>(null, 'get', url, requestOptions);
    }
    // get: api/staffTypes/${encodeURIComponent(id)}
    public RouteGet = (id: string) => `api/staffTypes/${encodeURIComponent(id)}`;
    public get(id: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<StaffTypeDTO>> {
        let url = this.RouteGet(id);
        return HttpApi.RestRequest<any, StaffTypeDTO>(null, 'get', url, requestOptions);
    }
    // post: api/staffTypes
    public RoutePost = (dto: StaffTypeDTO) => `api/staffTypes`;
    public post(dto: StaffTypeDTO, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<string>> {
        let url = this.RoutePost(dto);
        return HttpApi.RestRequest<any, string>(dto, 'post', url, requestOptions);
    }
    // put: api/staffTypes/toggle/${encodeURIComponent(id)}
    public RouteToggle = (id: string) => `api/staffTypes/toggle/${encodeURIComponent(id)}`;
    public toggle(id: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<void>> {
        let url = this.RouteToggle(id);
        return HttpApi.RestRequest<any, void>(null, 'put', url, requestOptions);
    }
}
var StaffTypeApiController = new StaffTypeApiControllerInternal();
export default StaffTypeApiController;

