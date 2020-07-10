import * as React from 'react';
import { Route, Switch, RouteComponentProps } from 'react-router-dom';
import TaskTable from './TaskTable';
import TaskEdit from './TaskEdit';
import Routes from 'config/ConfigureRoutes';
import PageNotFound from 'pages/public/PageNotFound';
import { UserState } from 'redux/UserReducer';
import ReduxStoreModel from 'redux/ReduxModel';
import { connect } from 'react-redux';
import { BreadcrumbsItem } from 'pages/shared/GlobalBreadcrumb';

interface TaskProps extends RouteComponentProps {
    User: UserState;
}

class Task extends React.Component<TaskProps, {}> {
    render() {
        return (
            <React.Fragment>
                <BreadcrumbsItem name="home" to={Routes.GET.BASE_ROUTE}>Tasks</BreadcrumbsItem>

                <Switch>
                    <Route exact path={Routes.GET.BASE_ROUTE} component={() => <TaskTable User={this.props.User} />} />
                    {/* <Route path={Routes.GET.TASK_EDIT} component={(props: any) => <TaskEdit {...props}  User={this.props.User} test={'hi'}/>} /> */}

                    {/* This needs to be the last item */}
                    <Route component={PageNotFound} />
                </Switch>
            </React.Fragment>
        );
    }
}

function mapStateToProps(state: ReduxStoreModel) {
    return {
        User: state.User,
    };
}

export default connect(mapStateToProps)(Task);
