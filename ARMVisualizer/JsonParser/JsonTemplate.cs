using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonParser
{
    public class VirtualMachinesVm0AdminPassword
    {
        public object defaultValue { get; set; }
        public string type { get; set; }
    }

    public class VirtualMachinesVm1AdminPassword
    {
        public object defaultValue { get; set; }
        public string type { get; set; }
    }

    public class VirtualMachinesVMB1AdminPassword
    {
        public object defaultValue { get; set; }
        public string type { get; set; }
    }

    public class VirtualMachinesVMB2AdminPassword
    {
        public object defaultValue { get; set; }
        public string type { get; set; }
    }

    public class AvailabilitySetsMyavlsetName
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class AvailabilitySetsMyavsetbName
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class VirtualMachinesVm0Name
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class VirtualMachinesVm1Name
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class VirtualMachinesVMB1Name
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class VirtualMachinesVMB2Name
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class LoadBalancersILBName
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class LoadBalancersLoadBalancerName
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class NetworkInterfacesVm0Name
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class NetworkInterfacesVm1Name
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class NetworkInterfacesVmb1465Name
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class NetworkInterfacesVmb2917Name
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class NetworkSecurityGroupsILBNSGName
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class PublicIPAddressesPublicIpName
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class VirtualNetworksVNETName
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class StorageAccountsSt4amzlfq5dm6eiName
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class LoadBalancersLoadBalancerId
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class LoadBalancersLoadBalancerId1
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class LoadBalancersLoadBalancerId2
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class LoadBalancersLoadBalancerId3
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class LoadBalancersLoadBalancerId4
    {
        public string defaultValue { get; set; }
        public string type { get; set; }
    }

    public class Parameters
    {
        public VirtualMachinesVm0AdminPassword virtualMachines_vm0_adminPassword { get; set; }
        public VirtualMachinesVm1AdminPassword virtualMachines_vm1_adminPassword { get; set; }
        public VirtualMachinesVMB1AdminPassword virtualMachines_VMB1_adminPassword { get; set; }
        public VirtualMachinesVMB2AdminPassword virtualMachines_VMB2_adminPassword { get; set; }
        public AvailabilitySetsMyavlsetName availabilitySets_myavlset_name { get; set; }
        public AvailabilitySetsMyavsetbName availabilitySets_myavsetb_name { get; set; }
        public VirtualMachinesVm0Name virtualMachines_vm0_name { get; set; }
        public VirtualMachinesVm1Name virtualMachines_vm1_name { get; set; }
        public VirtualMachinesVMB1Name virtualMachines_VMB1_name { get; set; }
        public VirtualMachinesVMB2Name virtualMachines_VMB2_name { get; set; }
        public LoadBalancersILBName loadBalancers_ILB_name { get; set; }
        public LoadBalancersLoadBalancerName loadBalancers_loadBalancer_name { get; set; }
        public NetworkInterfacesVm0Name networkInterfaces_vm0_name { get; set; }
        public NetworkInterfacesVm1Name networkInterfaces_vm1_name { get; set; }
        public NetworkInterfacesVmb1465Name networkInterfaces_vmb1465_name { get; set; }
        public NetworkInterfacesVmb2917Name networkInterfaces_vmb2917_name { get; set; }
        public NetworkSecurityGroupsILBNSGName networkSecurityGroups_ILBNSG_name { get; set; }
        public PublicIPAddressesPublicIpName publicIPAddresses_publicIp_name { get; set; }
        public VirtualNetworksVNETName virtualNetworks_VNET_name { get; set; }
        public StorageAccountsSt4amzlfq5dm6eiName storageAccounts_st4amzlfq5dm6ei_name { get; set; }
        public LoadBalancersLoadBalancerId loadBalancers_loadBalancer_id { get; set; }
        public LoadBalancersLoadBalancerId1 loadBalancers_loadBalancer_id_1 { get; set; }
        public LoadBalancersLoadBalancerId2 loadBalancers_loadBalancer_id_2 { get; set; }
        public LoadBalancersLoadBalancerId3 loadBalancers_loadBalancer_id_3 { get; set; }
        public LoadBalancersLoadBalancerId4 loadBalancers_loadBalancer_id_4 { get; set; }
    }

    public class Variables
    {
    }

    public class AvailabilitySet
    {
        public string id { get; set; }
    }

    public class HardwareProfile
    {
        public string vmSize { get; set; }
    }

    public class ImageReference
    {
        public string publisher { get; set; }
        public string offer { get; set; }
        public string sku { get; set; }
        public string version { get; set; }
    }

    public class Vhd
    {
        public string uri { get; set; }
    }

    public class OsDisk
    {
        public string name { get; set; }
        public string createOption { get; set; }
        public Vhd vhd { get; set; }
        public string caching { get; set; }
    }

    public class StorageProfile
    {
        public ImageReference imageReference { get; set; }
        public OsDisk osDisk { get; set; }
        public List<object> dataDisks { get; set; }
    }

    public class WindowsConfiguration
    {
        public bool provisionVMAgent { get; set; }
        public bool enableAutomaticUpdates { get; set; }
    }

    public class OsProfile
    {
        public string computerName { get; set; }
        public string adminUsername { get; set; }
        public WindowsConfiguration windowsConfiguration { get; set; }
        public List<object> secrets { get; set; }
        public string adminPassword { get; set; }
    }

    public class NetworkInterface
    {
        public string id { get; set; }
    }

    public class NetworkProfile
    {
        public List<NetworkInterface> networkInterfaces { get; set; }
    }

    public class Subnet
    {
        public string id { get; set; }
    }

    public class PublicIPAddress
    {
        public string id { get; set; }
    }

    public class Properties2
    {
        public string privateIPAddress { get; set; }
        public string privateIPAllocationMethod { get; set; }
        public Subnet subnet { get; set; }
        public PublicIPAddress publicIPAddress { get; set; }
    }

    public class FrontendIPConfiguration
    {
        public string name { get; set; }
        public Properties2 properties { get; set; }
    }

    public class BackendAddressPool
    {
        public string name { get; set; }
    }

    public class Subnet2
    {
        public string id { get; set; }
    }

    public class LoadBalancerBackendAddressPool
    {
        public string id { get; set; }
    }

    public class LoadBalancerInboundNatRule
    {
        public string id { get; set; }
    }

    public class Properties3
    {
        public string privateIPAddress { get; set; }
        public string privateIPAllocationMethod { get; set; }
        public Subnet2 subnet { get; set; }
        public List<LoadBalancerBackendAddressPool> loadBalancerBackendAddressPools { get; set; }
        public List<LoadBalancerInboundNatRule> loadBalancerInboundNatRules { get; set; }
    }

    public class IpConfiguration
    {
        public string name { get; set; }
        public Properties3 properties { get; set; }
    }

    public class DnsSettings
    {
        public List<object> dnsServers { get; set; }
        public string domainNameLabel { get; set; }
    }

    public class Properties4
    {
        public string protocol { get; set; }
        public string sourcePortRange { get; set; }
        public string destinationPortRange { get; set; }
        public string sourceAddressPrefix { get; set; }
        public string destinationAddressPrefix { get; set; }
        public string access { get; set; }
        public int priority { get; set; }
        public string direction { get; set; }
        public string description { get; set; }
    }

    public class SecurityRule
    {
        public string name { get; set; }
        public Properties4 properties { get; set; }
    }

    public class AddressSpace
    {
        public List<string> addressPrefixes { get; set; }
    }

    public class NetworkSecurityGroup
    {
        public string id { get; set; }
    }

    public class Properties5
    {
        public string addressPrefix { get; set; }
        public NetworkSecurityGroup networkSecurityGroup { get; set; }
    }

    public class Subnet3
    {
        public string name { get; set; }
        public Properties5 properties { get; set; }
    }

    public class Properties
    {
        public int platformUpdateDomainCount { get; set; }
        public int platformFaultDomainCount { get; set; }
        public AvailabilitySet availabilitySet { get; set; }
        public HardwareProfile hardwareProfile { get; set; }
        public StorageProfile storageProfile { get; set; }
        public OsProfile osProfile { get; set; }
        public NetworkProfile networkProfile { get; set; }
        public List<FrontendIPConfiguration> frontendIPConfigurations { get; set; }
        public List<BackendAddressPool> backendAddressPools { get; set; }
        public List<object> loadBalancingRules { get; set; }
        public List<object> probes { get; set; }
        public List<object> inboundNatRules { get; set; }
        public List<object> outboundNatRules { get; set; }
        public List<object> inboundNatPools { get; set; }
        public List<IpConfiguration> ipConfigurations { get; set; }
        public DnsSettings dnsSettings { get; set; }
        public bool? enableIPForwarding { get; set; }
        public List<SecurityRule> securityRules { get; set; }
        public string publicIPAllocationMethod { get; set; }
        public int? idleTimeoutInMinutes { get; set; }
        public AddressSpace addressSpace { get; set; }
        public List<Subnet3> subnets { get; set; }
        public string accountType { get; set; }
    }

    public class Tags
    {
    }

    public class Resource
    {
        public string comments { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string apiVersion { get; set; }
        public string location { get; set; }
        public Properties properties { get; set; }
        public List<object> dependsOn { get; set; }
        public Tags tags { get; set; }
    }

    public class RootObject
    {
        public string _schema { get; set; }
        public string contentVersion { get; set; }
        public Parameters parameters { get; set; }
        public Variables variables { get; set; }
        public List<Resource> resources { get; set; }
    }
}
