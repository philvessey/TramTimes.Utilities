---
name: 🚊 New Tram Network Support
about: Request or propose support for a new tram network
title: '[NETWORK] '
labels: ['enhancement', 'network-support', 'triage']
assignees: ''
---

## 🚊 Tram Network Information

**Network Name:**

**Location:**

**Operator:**

**Official Website:**

## 🌐 Data Availability

**Is schedule/timetable data available?**

- [ ] Yes
- [ ] No
- [ ] Unknown

**Schedule Data Details:**

- Data Type: [e.g., GTFS, custom format]
- Data Source: 
- Update Frequency: 
- Data Format: 

## 🚏 Stop Information

**Number of stops/stations:**

**Stop ID format:**

Example stop IDs:
- 
- 

## 📋 Service Information

**Operating Hours:**

**Service Types:**

- [ ] Regular service
- [ ] Express service
- [ ] Night service
- [ ] Weekend service
- [ ] Other: _______________

**Number of lines/routes:**

## 🔧 Technical Requirements

**What utilities would need to be updated?**

- [ ] TramTimes.Cache.Stops
- [ ] TramTimes.Database.Schedules
- [ ] TramTimes.Database.Stops
- [ ] TramTimes.Search.Stops
- [ ] All utilities

**Special considerations:**

List any unique characteristics of this network:
- 
- 

## 📦 Data Sample

**Can you provide sample data?**

If possible, provide sample stop IDs, API responses, or schedule data:

```json
{
  "sample": "data here"
}
```

## 🔍 Additional Context

Add any other context about this tram network:

- Links to developer resources
- Similar networks already supported
- Known challenges or limitations
- Community interest level

## 🙋 Contribution

**Are you willing to help implement this?**

- [ ] I can provide data samples
- [ ] I can help with testing
- [ ] I can help with implementation
- [ ] I can help with documentation
- [ ] I can provide local knowledge
- [ ] I'm just requesting, not able to contribute

## ✔️ Checklist

- [ ] I have checked if this network is already supported
- [ ] I have checked for existing issues requesting this network
- [ ] I have provided data availability information
- [ ] I have provided example stop IDs or data
- [ ] I have read the [CONTRIBUTING](https://github.com/philvessey/TramTimes.Utilities/blob/master/CONTRIBUTING.md) guidelines