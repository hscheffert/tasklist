import React from 'react';
import { Space, notification, } from 'antd';
import HistoryUtil from '../../utils/HistoryUtil';
import Routes from 'config/ConfigureRoutes';
import SubAreaDTO from 'models/generated/SubAreaDTO';
import DataTable, { DataTableColumnProps } from '../shared/DataTable';
import DataTableUtil from '../../utils/DataTableUtil';
import SubAreaApiController from 'api/SubAreaApiController';

interface SubAreaTableProps {
    subAreas: SubAreaDTO[];
    areaId: string;
    areaName: string;    
}

interface SubAreaTableState {
    subAreas: SubAreaDTO[];
    loading: boolean;
}

class SubAreaTable extends React.Component<SubAreaTableProps, SubAreaTableState> {
    private dataTable: DataTable<SubAreaDTO>;
    private customTableColumns: DataTableColumnProps<any>[] = [
        DataTableUtil.Columns.DisplayOrder(true),
        DataTable.StandardColumns.Text('Name', 'name'),
    ];

    constructor(props: SubAreaTableProps) {
        super(props);
        this.state = {
            loading: false,
            subAreas: this.props.subAreas,
        };
    }

    componentDidMount() {
        this.customTableColumns.push(DataTableUtil.Columns.Active(this.handleActiveToggle));
        this.customTableColumns.push(DataTableUtil.Columns.Actions('subAreaId', this.editSubArea));
        this.fetchData();
    }

    componentDidUpdate(prevProps:SubAreaTableProps) {
        if(!prevProps.subAreas.length && this.props.subAreas.length) {
            this.dataTable.refresh();
        }
    }

    renderTable = () => {
        const addButton = DataTable.TableButtons.Add("Add Sub Area", Routes.SUBAREA_EDIT('0', this.props.areaId).ToRoute());

        return (
            <DataTable
                ref={(ele: any) => this.dataTable = ele}
                serverSide={false}
                tableProps={{
                    rowKey: 'subAreaId',
                    loading: this.state.loading,
                    sortDirections: ['ascend', 'descend'],
                    pagination: false
                }}
                columns={this.customTableColumns}
                data={this.state.subAreas}
                globalSearch={true}
                buttonBar={[addButton]}
            />
        );
    }

    render() {
        return (
            <Space direction="vertical" style={{ width: '100%' }} size={'small'}>
                {this.renderTable()}
            </Space>
        );
    }

    private editSubArea = (subAreaId: string | null) => {
        HistoryUtil.push(Routes.SUBAREA_EDIT(subAreaId as string, this.props.areaId).ToRoute());
    }

    private handleActiveToggle = async (subArea: SubAreaDTO) => {
        if (subArea.subAreaId == null) return;

        this.setState({ loading: true });

        try {
            const result = await SubAreaApiController.toggle(subArea.subAreaId);

            this.setState({ loading: false });
            this.fetchData();
        } catch (err) {
            this.setState({ loading: false });
            console.error(err);
            notification.error({
                message: err.message,
                description: err.description
            });
        }
    }

    private fetchData = async () => {
        if(this.props.areaId === '0') return;

        this.setState({ loading: true });

        try {
            const result = await SubAreaApiController.getAllByAreaId(this.props.areaId);

            this.setState({
                loading: false,
                subAreas: result.data
            });

            this.dataTable.refresh();
        } catch (err) {
            this.setState({ loading: false });
            console.error(err);
            notification.error({
                message: err.message,
                description: err.description
            });
        }
    }
}

export default SubAreaTable;
