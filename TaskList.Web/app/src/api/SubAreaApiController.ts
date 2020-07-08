import { HttpApi, HttpApiRequestOptions, AxiosResponse } from './ApiHelper';
import SubAreaDTO from '../models/generated/SubAreaDTO';


class SubAreaApiControllerInternal {
    // get: api/subAreas
    public RouteGetAll = () => `api/subAreas`;
    public getAll(requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<SubAreaDTO[]>> {
        let url = this.RouteGetAll();
        return HttpApi.RestRequest<any, SubAreaDTO[]>(null, 'get', url, requestOptions);
    }
    // get: api/subAreas/${encodeURIComponent(id)}
    public RouteGet = (id: string) => `api/subAreas/${encodeURIComponent(id)}`;
    public get(id: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<SubAreaDTO>> {
        let url = this.RouteGet(id);
        return HttpApi.RestRequest<any, SubAreaDTO>(null, 'get', url, requestOptions);
    }
    // post: api/subAreas
    public RoutePost = (dto: SubAreaDTO) => `api/subAreas`;
    public post(dto: SubAreaDTO, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<string>> {
        let url = this.RoutePost(dto);
        return HttpApi.RestRequest<any, string>(dto, 'post', url, requestOptions);
    }
    // put: api/subAreas/toggle/${encodeURIComponent(id)}
    public RouteToggle = (id: string) => `api/subAreas/toggle/${encodeURIComponent(id)}`;
    public toggle(id: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<void>> {
        let url = this.RouteToggle(id);
        return HttpApi.RestRequest<any, void>(null, 'put', url, requestOptions);
    }
}
var SubAreaApiController = new SubAreaApiControllerInternal();
export default SubAreaApiController;

