import { HttpApi, HttpApiRequestOptions, AxiosResponse } from './ApiHelper';


class ErrorControllerInternal {
    // post: /error
    public RouteError = () => `/error`;
    public error(requestOptions?: HttpApiRequestOptions): Promise<AxiosResponse<void>> {
        let url = this.RouteError();
        return HttpApi.RestRequest<any, void>(null, 'post', url, requestOptions);
    }
}
var ErrorController = new ErrorControllerInternal();
export default ErrorController;

