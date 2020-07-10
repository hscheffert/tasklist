import { HttpApi, HttpApiRequestOptions, AxiosResponse } from './ApiHelper';
import StaffDTO from '../models/generated/StaffDTO';
import TaskStaffDTO from '../models/generated/TaskStaffDTO';


class StaffApiControllerInternal {
    // get: api/staff
    public RouteGetAll = () => `api/staff`;
    public getAll(requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<StaffDTO[]>> {
        let url = this.RouteGetAll();
        return HttpApi.RestRequest<any, StaffDTO[]>(null, 'get', url, requestOptions);
    }
    // get: api/staff/getAllByTaskId/${encodeURIComponent(taskId)}
    public RouteGetAllByTaskId = (taskId: string) => `api/staff/getAllByTaskId/${encodeURIComponent(taskId)}`;
    public getAllByTaskId(taskId: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<TaskStaffDTO[]>> {
        let url = this.RouteGetAllByTaskId(taskId);
        return HttpApi.RestRequest<any, TaskStaffDTO[]>(null, 'get', url, requestOptions);
    }
    // get: api/staff/getAllActive
    public RouteGetAllActive = () => `api/staff/getAllActive`;
    public getAllActive(requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<StaffDTO[]>> {
        let url = this.RouteGetAllActive();
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
    // delete: api/staff/${encodeURIComponent(id)}
    public RouteDelete = (id: string) => `api/staff/${encodeURIComponent(id)}`;
    public delete(id: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<void>> {
        let url = this.RouteDelete(id);
        return HttpApi.RestRequest<any, void>(null, 'delete', url, requestOptions);
    }
}
var StaffApiController = new StaffApiControllerInternal();
export default StaffApiController;

