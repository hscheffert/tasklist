import * as React from 'react';
import { Route, Switch } from 'react-router-dom';
import TaskTable from './TaskTable';
import TaskEdit from './TaskEdit';
import Routes from 'config/ConfigureRoutes';
import PageNotFound from 'pages/public/PageNotFound';
import { UserState } from 'redux/UserReducer';
import { Dispatch, Action } from 'redux';
import ReduxStoreModel from 'redux/ReduxModel';
import { connect } from 'react-redux';

interface TaskProps {
    User: UserState;
}

class Task extends React.Component<TaskProps> {
    render() {
        return (
            <Switch>
                <Route exact path={Routes.GET.BASE_ROUTE} component={() => <TaskTable User={this.props.User} />} />
                <Route path={Routes.GET.TASK_EDIT} component={TaskEdit} />

                {/* This needs to be the last item */}
                <Route component={PageNotFound} />
            </Switch>
        );
    }
}

function mapStateToProps(state: ReduxStoreModel) {
    return {
        User: state.User,
    };
}

export default connect(mapStateToProps)(Task);
