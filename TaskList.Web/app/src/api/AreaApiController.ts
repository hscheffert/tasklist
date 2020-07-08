import { HttpApi, HttpApiRequestOptions, AxiosResponse } from './ApiHelper';
import AreaDTO from '../models/generated/AreaDTO';


class AreaApiControllerInternal {
    // get: api/areas
    public RouteGetAll = () => `api/areas`;
    public getAll(requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<AreaDTO[]>> {
        let url = this.RouteGetAll();
        return HttpApi.RestRequest<any, AreaDTO[]>(null, 'get', url, requestOptions);
    }
    // get: api/areas/${encodeURIComponent(id)}
    public RouteGet = (id: string) => `api/areas/${encodeURIComponent(id)}`;
    public get(id: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<AreaDTO>> {
        let url = this.RouteGet(id);
        return HttpApi.RestRequest<any, AreaDTO>(null, 'get', url, requestOptions);
    }
    // post: api/areas
    public RoutePost = (dto: AreaDTO) => `api/areas`;
    public post(dto: AreaDTO, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<string>> {
        let url = this.RoutePost(dto);
        return HttpApi.RestRequest<any, string>(dto, 'post', url, requestOptions);
    }
    // put: api/areas/toggle/${encodeURIComponent(id)}
    public RouteToggle = (id: string) => `api/areas/toggle/${encodeURIComponent(id)}`;
    public toggle(id: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<void>> {
        let url = this.RouteToggle(id);
        return HttpApi.RestRequest<any, void>(null, 'put', url, requestOptions);
    }
}
var AreaApiController = new AreaApiControllerInternal();
export default AreaApiController;

