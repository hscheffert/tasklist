import { HttpApi, HttpApiRequestOptions, AxiosResponse } from './ApiHelper';
import TaskDetailsDTO from '../models/generated/TaskDetailsDTO';
import TaskDTO from '../models/generated/TaskDTO';


class TaskApiControllerInternal {
    // get: api/tasks
    public RouteGetAll = () => `api/tasks`;
    public getAll(requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<TaskDTO[]>> {
        let url = this.RouteGetAll();
        return HttpApi.RestRequest<any, TaskDTO[]>(null, 'get', url, requestOptions);
    }
    // get: api/tasks/getAllUserTasks/${encodeURIComponent(id)}
    public RouteGetAllUserTasks = (id?: string) => `api/tasks/getAllUserTasks/${encodeURIComponent(id)}`;
    public getAllUserTasks(id?: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<TaskDTO[]>> {
        let url = this.RouteGetAllUserTasks(id);
        return HttpApi.RestRequest<any, TaskDTO[]>(null, 'get', url, requestOptions);
    }
    // get: api/tasks/${encodeURIComponent(id)}
    public RouteGet = (id: string) => `api/tasks/${encodeURIComponent(id)}`;
    public get(id: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<TaskDTO>> {
        let url = this.RouteGet(id);
        return HttpApi.RestRequest<any, TaskDTO>(null, 'get', url, requestOptions);
    }
    // get: api/tasks/getWithDetails/${encodeURIComponent(id)}
    public RouteGetWithDetails = (id: string) => `api/tasks/getWithDetails/${encodeURIComponent(id)}`;
    public getWithDetails(id: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<TaskDetailsDTO>> {
        let url = this.RouteGetWithDetails(id);
        return HttpApi.RestRequest<any, TaskDetailsDTO>(null, 'get', url, requestOptions);
    }
    // post: api/tasks
    public RoutePost = (dto: TaskDetailsDTO) => `api/tasks`;
    public post(dto: TaskDetailsDTO, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<string>> {
        let url = this.RoutePost(dto);
        return HttpApi.RestRequest<any, string>(dto, 'post', url, requestOptions);
    }
    // put: api/tasks/toggle/${encodeURIComponent(id)}
    public RouteToggle = (id: string) => `api/tasks/toggle/${encodeURIComponent(id)}`;
    public toggle(id: string, requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<void>> {
        let url = this.RouteToggle(id);
        return HttpApi.RestRequest<any, void>(null, 'put', url, requestOptions);
    }
}
var TaskApiController = new TaskApiControllerInternal();
export default TaskApiController;

