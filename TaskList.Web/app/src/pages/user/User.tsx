import * as React from 'react';
import { Route, Switch } from 'react-router-dom';
import Routes from 'config/ConfigureRoutes';
import PageNotFound from 'pages/public/PageNotFound';
import UserTable from './UserTable';
import UserEdit from './UserEdit';

class User extends React.Component<{}> {
    render() {        
        return (
            <Switch>
                <Route exact path={Routes.GET.USER_BASE} component={UserTable} />
                <Route exact path={Routes.GET.USER_EDIT} component={UserEdit} />

                {/* This needs to be the last item */}
                <Route component={PageNotFound} />
            </Switch>
        );
    }
}

export default User;
