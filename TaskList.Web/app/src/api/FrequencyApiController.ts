import { HttpApi, HttpApiRequestOptions, AxiosResponse } from './ApiHelper';
import FrequencyDTO from '../models/generated/FrequencyDTO';


class FrequencyApiControllerInternal {
    // get: api/frequencies
    public RouteGetAll = () => `api/frequencies`;
    public getAll(requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<FrequencyDTO[]>> {
        let url = this.RouteGetAll();
        return HttpApi.RestRequest<any, FrequencyDTO[]>(null, 'get', url, requestOptions);
    }
    // get: api/frequencies/${encodeURIComponent(id)}
    public RouteGet = (id: string) => `api/frequencies/${encodeURIComponent(id)}`;
    public get(id: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<FrequencyDTO>> {
        let url = this.RouteGet(id);
        return HttpApi.RestRequest<any, FrequencyDTO>(null, 'get', url, requestOptions);
    }
    // post: api/frequencies
    public RoutePost = (dto: FrequencyDTO) => `api/frequencies`;
    public post(dto: FrequencyDTO, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<string>> {
        let url = this.RoutePost(dto);
        return HttpApi.RestRequest<any, string>(dto, 'post', url, requestOptions);
    }
    // put: api/frequencies/toggle/${encodeURIComponent(id)}
    public RouteToggle = (id: string) => `api/frequencies/toggle/${encodeURIComponent(id)}`;
    public toggle(id: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<void>> {
        let url = this.RouteToggle(id);
        return HttpApi.RestRequest<any, void>(null, 'put', url, requestOptions);
    }
}
var FrequencyApiController = new FrequencyApiControllerInternal();
export default FrequencyApiController;

