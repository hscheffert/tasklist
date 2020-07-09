import * as React from 'react';
import { Switch } from 'react-router-dom';
import Routes from 'config/ConfigureRoutes';
import LayoutComponent from 'core/LayoutComponent';

/* Layouts */
import PublicLayout from 'layouts/PublicLayout';
import MainLayout from 'layouts/MainLayout';

/* Pages */
import PageNotFound from 'pages/public/PageNotFound';
import Frequency from 'pages/frequency/Frequency';
import Task from 'pages/task/Task';
import Area from 'pages/area/Area';
import StaffType from 'pages/staffType/StaffType';
import User from 'pages/user/User';
import TaskEdit from 'pages/task/TaskEdit';
import Unauthorized from 'pages/Unauthorized';
import LoggedOut from 'pages/LoggedOut';

/** Location to where the user will be redirected when found to be unauthorized */
const unauthorizedLocation: string = Routes.GET.UNAUTHORIZED;

/** Location to where the user will be redirected when logout needs to happen */
const logoutLocation: string = Routes.GET.LOGOUT;

const RouteLoader = <Switch>
    {/* Task is weird because this is our "base" route as well...so adding task edit here too */}
    <LayoutComponent exact path={Routes.GET.BASE_ROUTE} component={Task} layout={MainLayout} />

    <LayoutComponent path={Routes.GET.TASK_EDIT} component={TaskEdit} layout={MainLayout} adminOnly={true}/>
    <LayoutComponent path={Routes.GET.FREQUENCY_BASE} component={Frequency} layout={MainLayout} adminOnly={true}/>
    <LayoutComponent path={Routes.GET.AREA_BASE} component={Area} layout={MainLayout} adminOnly={true} />
    <LayoutComponent path={Routes.GET.STAFF_TYPE_BASE} component={StaffType} layout={MainLayout} adminOnly={true}/>
    <LayoutComponent path={Routes.GET.USER_BASE} component={User} layout={MainLayout} adminOnly={true} />

    <LayoutComponent exact path={Routes.GET.LOGGED_OUT} allowAnonymous={true} component={LoggedOut} layout={PublicLayout} />

    {/* Error Handling */}
    <LayoutComponent exact path={Routes.GET.PAGE_NOT_FOUND} component={PageNotFound} layout={PublicLayout} />
    <LayoutComponent exact path={Routes.GET.UNAUTHORIZED} allowAnonymous={true} component={Unauthorized} layout={PublicLayout} />

    {/* This needs to be the last item. Path also needs to be undefined */}
    <LayoutComponent path={undefined} component={PageNotFound} layout={PublicLayout} />
</Switch>;

export { RouteLoader, unauthorizedLocation, logoutLocation };
