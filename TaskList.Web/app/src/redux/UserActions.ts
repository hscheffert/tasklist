import { Dispatch } from 'redux';
import { AxiosResponse, AxiosError } from 'axios';
import BaseAction from 'models/frontend/common/BaseAction';
import { LoadingStatusType } from 'models/frontend/common/LoadingStatusType';
import UserDTO from 'models/generated/UserDTO';
import ActionResultDTO from 'models/frontend/common/ActionResultDTO';
import AuthController from 'api/AuthController';
import Routes from 'config/ConfigureRoutes';
import History from 'utils/HistoryUtil';
import CurrentUserDTO from '../models/generated/CurrentUserDTO';
import { notification } from 'antd';

export interface LoginUserAction extends BaseAction { type: 'LOGIN_USER'; data: UserDTO; }
export interface ClearLoginUserAction extends BaseAction { type: 'CLEAR_LOGIN_STATE'; }
export interface UpdateUserAction extends BaseAction { type: 'UPDATE_USER'; data: UserDTO; }
export interface UpdateUserStateAction extends BaseAction { type: 'UPDATE_USER_STATE'; data: LoadingStatusType; }

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
// JB: You have to have atleast 2 in here for everything to work. It's a typescript thing
export type KnownActions = LoginUserAction | ClearLoginUserAction | UpdateUserAction | UpdateUserStateAction;

export default class UserAction {
    constructor() {
        // Dont be that guy
        throw new Error("NOOOO");
    }

    public static Login(dispatch: Dispatch<KnownActions>) {
        dispatch({ type: "UPDATE_USER_STATE", data: "loading" } as UpdateUserStateAction);

        // return AccountController.PostLogin(request)
        //     .then(result => this.Login_OnSuccess(dispatch, result))
        //     .catch(error => this.Login_OnFailure(dispatch, error));
        window.location.href = 'api/auth/login';
    }

    /**
     * Login using the browser cookie. This will run through the login process as if we had logged in
     */
    public static SoftLogin(dispatch: Dispatch<KnownActions>): Promise<ActionResultDTO> {
        dispatch({ type: "UPDATE_USER_STATE", data: "loading" } as UpdateUserStateAction);
        // return AccountController.GetMe()
        //     .then(result => this.Login_OnSuccess(dispatch, result))
        //     .catch(error => this.Login_OnFailure(dispatch, error));

        return AuthController.getMe()
             .then(result => this.Login_OnSuccess(dispatch, result))
             .catch(error => this.Login_OnFailure(dispatch, error));
    }

    private static Login_OnSuccess(dispatch: Dispatch<KnownActions>, response: AxiosResponse<UserDTO>): ActionResultDTO {
        console.log(response.data);
        const data = UserDTO.create({
            ...response.data
        });

        dispatch({ type: "LOGIN_USER", data: data } as LoginUserAction);
        dispatch({ type: "UPDATE_USER_STATE", data: "finished" } as UpdateUserStateAction);
        return { isError: false };
    }

    private static Login_OnFailure(dispatch: Dispatch<KnownActions>, error: AxiosError): ActionResultDTO {
        dispatch({ type: "CLEAR_LOGIN_STATE" } as ClearLoginUserAction);
        dispatch({ type: "UPDATE_USER_STATE", data: "failed" } as UpdateUserStateAction);
        let messages = error != null && error.response != null && error.response.data.messages != null
            ? error.response.data.messages
            : ["Critical Error"];
        return { isError: true, message: messages.join("\n") };
    }

    public static Logout(dispatch: Dispatch<KnownActions>) {
        return fetch('https://localhost:44335/signout-oidc')
        .catch((error) => {
            console.log(error);
            notification.error({
                message: error.message,
                description: error.description
            });
            // this.Logout_OnComplete(dispatch);
        })
        .then(result => {
            this.Logout_OnComplete(dispatch);
            History.push(Routes.GET.LOGGED_OUT);
        });
    }

    private static Logout_OnComplete(dispatch: Dispatch<KnownActions>) {
        dispatch({ type: "CLEAR_LOGIN_STATE" } as ClearLoginUserAction);
        dispatch({ type: "UPDATE_USER_STATE", data: "none" } as UpdateUserStateAction);
    }
}
